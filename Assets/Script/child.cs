using UnityEngine;

public class child : Enemy
{
    public bool pedang = true;
    public override void Serang()
    {
        if (pedang)
        {
            Debug.Log("Child menyerang dengan pedang!");
        }
        else
        {
            Debug.Log("Child menyerang tanpa pedang!");
        }
    }
}
