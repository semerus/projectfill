using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GiraffeStar;

public class Vertex : InputBehaviour {
	public int Id { get; private set; }
	//bool isDragging;
	StageMakerModule module;

	public void Start()
	{
        module = GiraffeSystem.FindModule<StageMakerModule>();
        Init();
	}

    void Init()
    {
        OnInputDown += () =>
        {
            module.dotProcessing = true;
        };

        OnInputUp += () =>
        {
            module.dotProcessing = false;

            if (module.IsSnapping)
            {
                Snap();
            }
        };

        OnDrag += () =>
        {
            Vector3 nextPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            transform.position = nextPos;
            module.ChangeDotPosition();
        };
    }

	public void SetVertex(int id)
	{
		Id = id;
	}

    public void Snap()
    {        
        var snappedX = Mathf.Round(transform.position.x);
        var snappedY = Mathf.Round(transform.position.y);

        transform.position = new Vector3(snappedX, snappedY);
    }

    /*****************************************************************/
    /*Eventsystem Interface*/

    /*
	#region IPointerDownHandler implementation

	public void OnPointerDown (PointerEventData eventData)
	{
		module.dotProcessing = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!isDragging) 
		{
			var pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos = new Vector3 (pos.x, pos.y, 0f);
			module.CreateDot (pos);
		}
		isDragging = false;
		module.dotProcessing = false;
	}

	#endregion
    */
    /*
    #region IBeginDragHandler implementation

    public void OnBeginDrag (PointerEventData eventData)
	{
		isDragging = true;
	}

	#endregion
    
	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		Vector3 nextPos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
		transform.position = nextPos;
		module.ChangeDotPosition ();

        Debug.Log("Dragging");
	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		module.dotProcessing = false;
	}

	#endregion
    */
	/*****************************************************************/
}
