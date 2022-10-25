using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class MapClickable : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private List<Material> materialList;

    public TournamentMap.Room room;
    private MapIconType iconType;

    private bool isClickable = false;
    public bool IsClickable { get => isClickable; set => isClickable = value; }

    public void Init(TournamentMap.Room room, MapIconType iconType)
    {
        this.room = room;
        this.iconType = iconType;
    }

    #region Animation

    [SerializeField] private bool doHoverAnim;
    [SerializeField] private float MaxHoverScale;
    [SerializeField] private AnimationCurve hoverAnimCurve;
    [SerializeField] private float hoverAnimDuration;

    private float scale;
    private bool animating;
    private Coroutine hoverAnimation;

    private void Start()
    {
        scale = transform.localScale.x;
    }

    private void OnMouseEnter()
    {
        if (!doHoverAnim)
            return;

        if (animating)
            StopCoroutine(hoverAnimation);

        hoverAnimation = StartCoroutine(HoverAnimationCo(MaxHoverScale));
    }

    private void OnMouseExit()
    {

        if (!doHoverAnim)
            return;

        if (animating)
            StopCoroutine(hoverAnimation);

        hoverAnimation = StartCoroutine(HoverAnimationCo(scale));
    }

    private IEnumerator HoverAnimationCo(float newScale)
    {
        animating = true;

        float startTime = Time.time;
        float endTime = Time.time + hoverAnimDuration;
        float startScale = transform.localScale.x;
        float endScale = newScale;
        float t, scale;

        while (Time.time < endTime)
        {
            t = Mathf.InverseLerp(startTime, endTime, Time.time);
            t = hoverAnimCurve.Evaluate(t);

            scale = Mathf.LerpUnclamped(startScale, endScale, t);

            transform.localScale = Vector3.one * scale;

            yield return null;
        }

        transform.localScale = Vector3.one * endScale;

        animating = false;
    }


    #endregion

    private void OnMouseUpAsButton()
    {
        if (IsClickable)
        {
            TournamentData.Instance.CurrentRoom = room;
            TournamentData.Instance.PassedRooms.Add(room);

            GlobalSeed.Instance.SetSeed(room.Seed);
            switch (iconType)
            {
                case MapIconType.RACE:
                case MapIconType.BOSS:
                    PartSceneLoader.isEvent = false;
                    SceneManager.LoadScene(1);
                    break;
                case MapIconType.GEAR:
                    PartSceneLoader.isEvent = true;
                    SceneManager.LoadScene(4);
                    break;
            }
        }
    }

    #region Material

    public void ChangeColor(MapIconColor color)
    {
        meshRenderer.material = materialList[(int)color];
    }

    #endregion

}

public enum MapIconType
{ 
    RACE,
    GEAR,
    BOSS
}

public enum MapIconColor
{ 
    POSSIBLE,
    LOCKED,
    DONE,
    CURRENT,
}
