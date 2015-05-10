using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour
{
    public bool selected;
    public bool selectionChange;
    public GameController gameMaster;

    // Use this for initialization
    protected abstract void Start();

    // Update is called once per frame
    protected abstract void Update();

    public abstract void queueOrder();
    public abstract void queueOrder(Vector3 destination);
}
