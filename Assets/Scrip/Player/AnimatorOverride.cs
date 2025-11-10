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
    }

    void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
    }
    void Awake()
    {
        anims = GetComponentsInChildren<Animator>();
        foreach (var anim in anims)
        {
            animators.Add(anim.name, anim);
        }
    }

    private void OnItemSelectedEvent(ItemDetails item, bool isSelected)
    {
        //WORKFLOW:不全不同工具的不同动画
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
