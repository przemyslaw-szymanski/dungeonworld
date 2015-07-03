using UnityEngine;
using System.Collections;

public class BaseGUI : MonoBehaviour
{
	public int MainWndID = 0;
	public Rect MainWndRect = new Rect(0, 0, 0, 0);
    protected bool m_visible = false;
    protected bool m_fullScreen = false;
    protected bool m_movableWindow = true;
	protected string m_controlName = "";
    protected Rect m_WndDragRect = new Rect(0, 0, 100, 20);

	protected static Player m_Player;

    void Awake()
    {
		m_Player = ApplicationManager.Singleton.GetPlayer();
		m_controlName = this.GetType().Name;
        OnAwake();
    }

    void Start()
    {
		MainWndID = GameGUI.Singleton.GenerateWindowId();
        OnStart();
    }

    void Update()
    {
        OnUpdate();
    }

    void OnGUI()
    {
        if (!m_visible)
            return;

        MainWndRect.width *= GameGUI.Singleton.GUIScale;
        MainWndRect.height *= GameGUI.Singleton.GUIScale;
        if (!m_movableWindow)
            m_WndDragRect.width = 0;
        else
            m_WndDragRect.width = MainWndRect.width;

		if(CheckMouseOver())
		{
			OnMouseOver();
			int result = CheckMouseClick();
			if(result >= 0)
			{
				OnMouseClick(result);
			}
		}
		
		GUI.SetNextControlName(m_controlName);
        OnResize();
        OnDraw();
    }
	
	bool CheckMouseOver()
	{
		return MainWndRect.Contains(Event.current.mousePosition);
	}
	
	int CheckMouseClick()
	{
		return Event.current.button;
	}
	
	public void Show(bool show)
	{
        if (FullScreen)
        {
            if (GameGUI.CurrGUI != null && GameGUI.CurrGUI != this)
                GameGUI.CurrGUI.Show(false);

            GameGUI.CurrGUI = this;
        }

		m_visible = show;
		if(show)
			OnShow();
		else
			OnHide();
	}

    public bool Visible
    {
        get { return m_visible; }
        set { Show(value); }
    }
	
	public virtual void SetFocus(bool set)
	{
		if(set)
		{
			GUI.BringWindowToFront(MainWndID);
		}
		else
		{
			GUI.BringWindowToBack(MainWndID);
		}
	}

    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnDraw() { }
	protected virtual void OnShow(){}
	protected virtual void OnHide(){}
    protected virtual void OnResize() { }
    protected virtual void OnAwake() { }
	protected virtual void OnMouseOver(){}
	protected virtual void OnMouseClick(int button) {}
	
	public Player Player
	{
		get { return m_Player; }
	}

    public bool FullScreen
    {
        get { return m_fullScreen; }
    }
}
