using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BehaviourBase
{
    public static GameManager s_instance = null;
    public static GameManager Instance { get { return s_instance; } }

    [SerializeField] DiceManager _diceManager;


    [SerializeField] UIManager _uiManager;//데니
    [SerializeField] DataManager _dataManager;
    public UIManager UI { get { return _uiManager; } }
    public DataManager Data { get { return _dataManager; } } //




    private void Init()
    {
        if (s_instance == null)
        {
            s_instance = this.GetComponent<GameManager>();
            UI.Init();
            //_dataManager.Init();
        }
    }

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }


#if UNITY_EDITOR
    protected override void OnBindSerializedField()
    {
        base.OnBindSerializedField();
        _diceManager = this.GetComponentInChildren<DiceManager>();
        _uiManager = this.GetComponentInChildren<UIManager>();
        _dataManager = this.GetComponentInChildren<DataManager>();
    }
#endif

}
