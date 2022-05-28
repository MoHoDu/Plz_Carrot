using UnityEngine;

public class Movement2D : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed = 0.0f;
	[SerializeField]
	private Vector3 moveDirection = Vector3.zero;
	private float baseMoveSpeed;
	private Vector3 postTransform;
	private int startPoint;
	private int wayIndex;
	Animator animator;

	private bool isDie = false;

	// moveSpeed 변수의 프로퍼티(Property) (Get 가능)
	public float MoveSpeed
	{
		set => moveSpeed = Mathf.Max(0, value);
		get => moveSpeed;
	}

	private void Awake()
	{
		baseMoveSpeed = moveSpeed;
		animator = gameObject.GetComponent<Animator>();
	}

	private void Update()
	{
		if (isDie != true)
        {
			postTransform = transform.position;
			transform.position += moveDirection * moveSpeed * Time.deltaTime;

			if (startPoint == 1)
            {
				animator.SetBool("isFirst", false);
			}

			if (postTransform.x - transform.position.x < 0)
			{
				animator.SetBool("isX", true);
				animator.SetFloat("moveX", 1);
			}
			else if (postTransform.x - transform.position.x == 0)
			{
				animator.SetBool("isX", false);
				animator.SetFloat("moveX", 0);
			}
			else
			{
				animator.SetBool("isX", true);
				animator.SetFloat("moveX", 0);
			}

			if (postTransform.y - transform.position.y < 0)
			{
				animator.SetFloat("moveY", 1);
			}
			else
			{
				animator.SetFloat("moveY", 0);
			}
        }


	}

	public void MoveTo(Vector3 direction, int currentIndex)
	{
		moveDirection = direction;

		if (currentIndex > 90)
			return;

		startPoint = currentIndex - 1;
	}

	public void ResetMoveSpeed()
	{
		moveSpeed = baseMoveSpeed;
	}

	public void onDie()
    {
		isDie = true;
    }
}
