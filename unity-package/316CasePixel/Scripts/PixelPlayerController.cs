using UnityEngine;

namespace Case316.Pixel
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PixelPlayerController : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 4f;
        [SerializeField] int moveActionCost = 1;
        [SerializeField] int interactActionCost = 2;
        [SerializeField] float moveSpendDistance = 1f;
        [SerializeField] ActionTimeManager actionTimeManager;
        [SerializeField] InteractionPromptUI promptUI;

        Rigidbody2D body;
        Vector2 input;
        IInteractable currentInteractable;
        Vector2 lastSpendPosition;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            lastSpendPosition = transform.position;
        }

        void Update()
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (input.sqrMagnitude > 1f)
            {
                input.Normalize();
            }

            if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
            {
                if (actionTimeManager == null || actionTimeManager.TrySpend(interactActionCost, "interact"))
                {
                    currentInteractable.Interact(this);
                }
            }
        }

        void FixedUpdate()
        {
            if (input == Vector2.zero)
            {
                body.velocity = Vector2.zero;
                return;
            }

            if (actionTimeManager != null && actionTimeManager.ActionPoints <= 0)
            {
                body.velocity = Vector2.zero;
                return;
            }

            body.velocity = input * moveSpeed;

            if (Vector2.Distance(lastSpendPosition, body.position) >= moveSpendDistance)
            {
                if (actionTimeManager != null && !actionTimeManager.TrySpend(moveActionCost, "move"))
                {
                    body.velocity = Vector2.zero;
                }

                lastSpendPosition = body.position;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                currentInteractable = interactable;
                promptUI?.Show(interactable.PromptText);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractable interactable) && interactable == currentInteractable)
            {
                currentInteractable = null;
                promptUI?.Hide();
            }
        }
    }
}
