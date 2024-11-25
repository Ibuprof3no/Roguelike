using UnityEngine;

public class TurnManager
{
    //algo que me cuente los turnos
    private int m_turnCount;
    public event System.Action OnTick;
        //singleton
    //algo que modifique/cambie los turnos

    //método al que podamos lllamar cada vez que pase un turno, para que sume turno
    public  TurnManager()
    {
        m_turnCount = 1;
        Debug.Log("Turno actual: " + m_turnCount);
    }
    public void Tick()
    {
        m_turnCount += 1;
        OnTick?.Invoke();
        Debug.Log("Turno actual: "+m_turnCount);
    }
}