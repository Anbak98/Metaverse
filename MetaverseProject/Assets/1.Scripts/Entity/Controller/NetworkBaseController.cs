using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkBaseController : NetworkBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private SpriteRenderer characterRenderer;
    protected Transform handPivot;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection => movementDirection;

    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection => lookDirection;

    private Vector2 knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleAction();
        SubmitRotateRequestRpc(lookDirection);
    }

    protected virtual void FixedUpdate()
    {
        SubmitVelocityRequestRpc(movementDirection);
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.deltaTime;
        }
    }

    protected virtual void HandleAction()
    {

    }

    [Rpc(SendTo.Server)]
    private void SubmitVelocityRequestRpc(Vector2 direction, RpcParams rpcParm = default)
    {
        direction *= 5;
        if(knockbackDuration > 0)
        {
            direction *= 0.2f;
            direction += knockback;
        }

        _rigidbody.velocity = direction;
    }

    [Rpc(SendTo.Server)]
    private void SubmitRotateRequestRpc(Vector2 direction, RpcParams rpcParm = default)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if(Mathf.Abs(rotZ) > 90f)
        {
            transform.rotation = Quaternion.Euler(0f, 180, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0, 0f);
        }

        if (handPivot != null)
        {
            handPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        knockback = (other.position - transform.position).normalized * power;
    }
}
