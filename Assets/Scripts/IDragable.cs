
using UnityEngine;
public interface IDragable 
{
    Transform SnapPoint { get; set; }
    bool IsTaken { get; set; }

    void OnBeginTake();
    void OnTake();
    void OnRelease();
    void OnThrow();

}
