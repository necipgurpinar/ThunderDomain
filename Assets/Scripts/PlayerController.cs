using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
     [SerializeField] float _movementSpeed;
  [SerializeField] float _jumpforce;
  [SerializeField] float movementX;
  [SerializeField] bool _thunderActive = false;
  [SerializeField] bool _isJump = true;
  [SerializeField] float _thunderRadius;
  [SerializeField] int _cooldown;
  [SerializeField] bool _skilll = true;
  [SerializeField] int _skillCooldown;
  [SerializeField] GameObject _circlePrefab;
  [SerializeField] GameObject _thunderEffect;


private float _circleRadius;
private GameObject _circleZoneInstance;
private Vector3 _circleCenter;
  private Vector3 _moveDirection;
  private Rigidbody rb;

  private Vector3 _thunderCenter;

  void Awake()
  {
    rb = GetComponent<Rigidbody>();

  }
  void Update()
  {
    MovePlayer();
    StartCoroutine(jump());
    StartCoroutine(ChechThunderDomain());

  }



  void MovePlayer()
  {
    float moveX = Input.GetAxisRaw("Horizontal");
    float moveZ = Input.GetAxisRaw("Vertical");

    _moveDirection = new Vector3(moveX, 0, moveZ).normalized;
    rb.linearVelocity = _moveDirection * _movementSpeed + new Vector3(0, rb.linearVelocity.y, 0);

    if (_moveDirection != Vector3.zero)
    {
      transform.forward = _moveDirection;
    }

  }

  IEnumerator jump()
  {

    if (Input.GetKeyDown(KeyCode.Space) && _isJump)
    {
      _isJump = false;
      rb.AddForce(Vector3.up * _jumpforce, ForceMode.Impulse);
      yield return new WaitForSeconds(2f);
      _isJump = true;
    }

  }

  
  IEnumerator ChechThunderDomain()
  {
    if (Input.GetKeyDown(KeyCode.E) && !_thunderActive && _skilll)
    {
      _skilll = false;
      _thunderCenter=transform.position;
      _circleCenter = new Vector3(transform.position.x,transform.position.y - 0.9f,transform.position.z);
      _thunderActive = true;
      _thunderEffect.SetActive(true);
      _circleZoneInstance = Instantiate(_circlePrefab,_circleCenter,Quaternion.identity);
      _circleZoneInstance.transform.localScale = new Vector3(_thunderRadius* 0.23f,1f,_thunderRadius * 0.23f);
      StartCoroutine(ThunderDomain());
      StartCoroutine(skillCooldown());

      
      
      if(_thunderActive)
      {
        yield return new WaitForSeconds(_cooldown);
        Destroy(_circleZoneInstance);
            _thunderActive = false;
        _thunderEffect.SetActive(false);
                                            
      }
          
    }
  }

  IEnumerator skillCooldown()
  {
    yield return new WaitForSeconds(_skillCooldown);
    _skilll = true;
    StartCoroutine(skillCooldown());
  }

    IEnumerator ThunderDomain()
  {
    
    while (_thunderActive )
    {
      float distance = Vector3.Distance(transform.position,_thunderCenter);
      float orginalspeed = _movementSpeed;
      if(distance <= _thunderRadius)
      {
        rb.linearVelocity = movementX * _moveDirection * _movementSpeed;
        yield return null;
      }
      else
      {
        _movementSpeed = orginalspeed;
      _thunderActive= false;
      StopCoroutine(ThunderDomain());
      Destroy(_circleZoneInstance);
      _thunderEffect.SetActive(false);
      
      
      }
      
    }
  }


  private void OnDrawGizmos()
  {
    if (_thunderActive)
    {
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(_thunderCenter, _thunderRadius);
    }
  
  }
}
