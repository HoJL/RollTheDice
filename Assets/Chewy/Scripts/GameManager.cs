using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] DiceManager _diceManager;
    [SerializeField] UIManager _uiManager;//데니
    [SerializeField] DataManager _dataManager;
    [SerializeField] PoolManager _poolManager;
    public UIManager UI { get { return _uiManager; } }
    public DataManager Data { get { return _dataManager; } }
    public PoolManager Pool { get { return _poolManager; } }
    public DiceManager DiceManager{get => _diceManager;}
    //
    private void Init()
    {
        if (_uiManager != null)
            _uiManager.Init();
        if (_poolManager != null)
            Pool.Init();
        if (_diceManager != null)
            _diceManager.Init();
    }

    void Start()
    {
        Init();
    }

#if UNITY_EDITOR
    protected override void OnBindSerializedField()
    {
        base.OnBindSerializedField();
        _diceManager = this.GetComponentInChildren<DiceManager>();
        _uiManager = this.GetComponentInChildren<UIManager>();
        _dataManager = this.GetComponentInChildren<DataManager>();
        _poolManager = this.GetComponentInChildren<PoolManager>();
    }
#endif

}
