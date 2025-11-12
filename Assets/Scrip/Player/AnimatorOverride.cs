using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOverride : MonoBehaviour
{
    private Animator[] anims;
    public SpriteRenderer holdItem;
    //存储所有部位的状态转换
    public List<AnimationType> animationTypes;
    //存储角色所有部位的Animator组件，并与名字一一对应
    public Dictionary<string, Animator> animators = new Dictionary<string, Animator>();

    void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.BeforeSceneUnLoadEvent += OnBeforeSceneUnLoadEvent;
    }


    void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.BeforeSceneUnLoadEvent -= OnBeforeSceneUnLoadEvent;
    }
    void Awake()
    {
        anims = GetComponentsInChildren<Animator>();
        foreach (var anim in anims)
        {
            animators.Add(anim.name, anim);
        }
    }

    private void OnBeforeSceneUnLoadEvent()
    {
        SwitchAnimatior(PartType.None);
        holdItem.enabled = false;
    }
    /// <summary>
    /// 传入装备工具信息，以用来切换动画
    /// </summary>
    /// <param name="item"></param>
    /// <param name="isSelected"></param>
    private void OnItemSelectedEvent(ItemDetails item, bool isSelected)
    {
        //WORKFLOW:补全全不同工具的不同动画
        PartType partType = item.itemType switch
        {
            ItemType.Seed => PartType.Hold,
            ItemType.Commodity => PartType.Hold,
            _ => PartType.None,
        };

        if (isSelected == false)
        {
            partType = PartType.None;
            holdItem.enabled = false;
        }
        else
        {
            holdItem.enabled = true;
            holdItem.sprite = item.itemInWorldSprite;
        }
        SwitchAnimatior(partType);
    }
    
    /// <summary>
    /// 切换动画
    /// </summary>
    /// <param name="partType">动画</param>
    private void SwitchAnimatior(PartType partType)
    {
        foreach(var anim in animationTypes)
        {
            if(anim.partType == partType)
            {
                animators[anim.partName.ToString()].runtimeAnimatorController = anim.overrideController;
            }
        }
    }
}
