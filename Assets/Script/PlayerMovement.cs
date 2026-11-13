
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerHealthBar))]
public class PlayerMovement : MonoBehaviour
{
    public float kecepatan = 5f;
    private Vector2 arahGerak;
    private GameManager gameManager;
    private Animator animator;

    public int skor = 0;

    void Awake()
    {
        if (GetComponent<PlayerHealth>() == null)
        {
            gameObject.AddComponent<PlayerHealth>();
        }

        if (GetComponent<PlayerHealthBar>() == null)
        {
            gameObject.AddComponent<PlayerHealthBar>();
        }
    }

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        animator = GetComponent<Animator>();
    }

  void OnMove(InputValue value)
{
    Vector2 inputBaru = value.Get<Vector2>();

    if (inputBaru == Vector2.zero)
    {
        animator.SetBool("iswalking", false);

        animator.SetFloat("lastinputx", arahGerak.x);
        animator.SetFloat("lastinputy", arahGerak.y);
    }
    else
    {
        animator.SetBool("iswalking", true);
    }

    arahGerak = inputBaru;

    animator.SetFloat("inputx", arahGerak.x);
    animator.SetFloat("inputy", arahGerak.y);
}

    void Update()
    {
        Vector3 arah = new Vector3(arahGerak.x, arahGerak.y, 0);

        transform.position += arah * kecepatan * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            skor++;
            Debug.Log("Skor: " + skor);

            if (gameManager != null)
            {
                gameManager.AmbilKoin();
            }

            if (other.GetComponent<Item>() == null)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
