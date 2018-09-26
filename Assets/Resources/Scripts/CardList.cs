using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardList : MonoBehaviour {
    public List<GameObject> RCardGroup = new List<GameObject>();
    public float RCardGroupHeightIncrement = 0.02f;
    public virtual GameObject DistributionRCard(GameObject AimArea, int pos = 1)
    {
        int AimRCardIndex = RCardGroup.Count - pos;//重头数
        GameObject TheRCard = RCardGroup[AimRCardIndex];
        TheRCard.GetComponent<RCard>().CardArea = AimArea.GetComponent<MapNode>().Area;
        RCardGroup.RemoveAt(AimRCardIndex);
        return TheRCard;
    }

    public virtual void AcquireRCard(GameObject card, int pos = 1)
    {
        RCardPosChange(card);
        RCardGroup.Insert(RCardGroup.Count - pos + 1, card);
    }

    public virtual void RCardPosChange(GameObject card)
    {
        GameObject obj = RCardGroup.Count == 0 ? this.gameObject : RCardGroup[RCardGroup.Count - 1];
        Vector3 pos = obj.transform.position;
        pos.y += RCardGroupHeightIncrement;
        card.transform.position = pos;
    }
}
