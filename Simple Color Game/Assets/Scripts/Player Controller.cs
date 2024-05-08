using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int SwapWeapon;

    public Animator Animator;
    

    public bool Dead;

    public PlayerInput MPI;
    private InputAction move;
    private InputAction restart;
    private InputAction quit;
    private InputAction swap;
    private InputAction attack;
    
    public float PlayerSpeed;
    public bool PlayerShouldBeMoving;

    public Rigidbody2D PlayerRB;
    private float moveDirection;

    public int Colliding;
    private float inputHorizontal;

    public GameObject WeaponOne;
    public GameObject WeaponTwo;
    public GameObject WeaponThree;

    public bool WeaponOneActive;
    public bool WeaponTwoActive;
    public bool WeaponThreeActive;

    public bool Attack;
    public bool CanAttack;

    public float AttackTimerOneMax;
    public float AttackTimerOne;
    public float AttackTimerTwoMax;
    public float AttackTimerTwo;
    public float AttackTimerThreeMax;
    public float AttackTimerThree;

    public bool CanSwap;
    public bool CanMove;

    // Start is called before the first frame update
    void Start()
    {
     

        Colliding = 0;
        PlayerRB = GetComponent<Rigidbody2D>();
        MPI = GetComponent<PlayerInput>();

        //Grabs all the player's inputs
        move = MPI.currentActionMap.FindAction("Move");
        restart = MPI.currentActionMap.FindAction("Restart");
        quit = MPI.currentActionMap.FindAction("Quit");
        swap = MPI.currentActionMap.FindAction("Swap");
        attack = MPI.currentActionMap.FindAction("Attack");

        MPI.currentActionMap.Enable();

        move.started += Handle_MoveStarted;
        move.canceled += Handle_MoveCanceled;
        restart.performed += Handle_RestartPerformed;
        quit.performed += Handle_QuitPerformed;
        swap.performed += Swap_Performed;
        attack.performed += Attack_Performed;

    }


    public void OnDestroy()
    {
        //Remove control when OnDestroy activates
        move.started -= Handle_MoveStarted;
        move.canceled -= Handle_MoveCanceled;
        restart.performed -= Handle_RestartPerformed;
        quit.performed -= Handle_QuitPerformed;
        swap.performed -= Swap_Performed;
        attack.performed -= Attack_Performed;
    }



        private void Swap_Performed(InputAction.CallbackContext obj)
    {
        if (CanSwap == true)
        {
            SwapWeapon++;
        }
    }


    private void Handle_MoveStarted(InputAction.CallbackContext obj)
    {
        if (CanMove == true)
        {


            if (Dead != true)
            {
                //Can only be active if dash isn't occuring

                //Turns on the movement command
                PlayerShouldBeMoving = true;

                print("Handled move Started");

            }
        }
    }
    private void Handle_MoveCanceled(InputAction.CallbackContext obj)
    {//Can only be active if dash isn't occuring
       
            //Turns off the movement command
            PlayerShouldBeMoving = false;
            //Turns off the movement animation
          
        PlayerRB.velocity = new Vector2(0, PlayerRB.velocity.y);
            print("Handled move Canceled");

        Animator.SetBool("Moving", false);
    }
   
    public void FixedUpdate()
    {
        if (CanMove == false)
        {
            PlayerRB.velocity = new Vector2(0, PlayerRB.velocity.y);
        }
        if (SwapWeapon == 1)
        {
            print("Weapon One Active");
            WeaponTwo.gameObject.SetActive(false);
            WeaponThree.gameObject.SetActive(false);
            

            WeaponOneActive = true;
            WeaponTwoActive = false;
            WeaponThreeActive = false;

        }
        if (SwapWeapon == 2)
        {
            print("Weapon Two Active");
            WeaponOne.gameObject.SetActive(false);
            WeaponThree.gameObject.SetActive(false);
            WeaponOneActive = false;
            WeaponTwoActive = true;
            WeaponThreeActive = false;
            
        }
        if (SwapWeapon == 3)
        {
            print("Weapon Three Active");
            WeaponOne.gameObject.SetActive(false);
            WeaponTwo.gameObject.SetActive(false);
            WeaponOneActive = false;
            WeaponTwoActive = false;
            WeaponThreeActive = true;
           
        }
        if (SwapWeapon == 4)
        {
            SwapWeapon = 1;
        }

        if (WeaponOneActive == true)
        {
            if (Attack == true)
            {
                AttackTimerOne -= Time.deltaTime;
                if (AttackTimerOne > 0)
                {
                    WeaponOne.gameObject.SetActive(true);
                    CanSwap = false;
                    PlayerRB.velocity = new Vector2(0, PlayerRB.velocity.y);
                    CanMove = false;
                }
                if (AttackTimerOne <= 0)
                {
                    WeaponOne.gameObject.SetActive(false);
                    AttackTimerOne = AttackTimerOneMax;
                    CanAttack = true;
                    CanSwap = true;
                    CanMove = true;
                    Attack = false;
                    
                }
            }
        }
        if (WeaponTwoActive == true)
        {
            if (Attack == true)
            {
                AttackTimerTwo -= Time.deltaTime;
                if (AttackTimerTwo > 0)
                {
                    WeaponTwo.gameObject.SetActive(true);
                    CanSwap = false;
                    PlayerRB.velocity = new Vector2(0, PlayerRB.velocity.y);
                    CanMove = false;
                }
                if (AttackTimerTwo <= 0)
                {
                    WeaponTwo.gameObject.SetActive(false);
                    AttackTimerTwo = AttackTimerTwoMax;
                    CanAttack = true;
                    CanSwap = true;
                    CanMove = true;
                    Attack = false;
                }
                
            }
        }
        if (WeaponThreeActive == true)
        {
             if (Attack == true)
             {
                AttackTimerThree -= Time.deltaTime;
                if (AttackTimerThree > 0)
                {
                    WeaponThree.gameObject.SetActive(true);
                    CanSwap = false;
                    PlayerRB.velocity = new Vector2(0, PlayerRB.velocity.y);
                    CanMove = false;
                }
                if (AttackTimerThree <= 0)
                {
                    WeaponThree.gameObject.SetActive(false);
                    AttackTimerThree = AttackTimerThreeMax;
                    CanAttack = true;
                    CanSwap = true;
                    CanMove = true;
                    Attack = false;
                }
                
             }
        }


            if (PlayerShouldBeMoving == true)
        {
            if (CanMove == true)
            {


                if (Dead != true)
                {
                    print("PlayerRB Should Be Moving");
                    //Makes the player able to move, and turns on the moving animation   
                    PlayerRB.velocity = new Vector2(PlayerSpeed * moveDirection, PlayerRB.velocity.y);
                    Animator.SetBool("Moving", true);
                }
            }  
            
        }
        

    }

    private void Attack_Performed(InputAction.CallbackContext obj)
    {
        if (CanAttack == true)
        {
            Attack = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.gameObject.tag == "Ground")//Checks if the player is touching the ground
        {
            //GroundSaver = true;
            print("Touch Grass");
            Colliding++;
            //IsColliding = true;
        }

     

        if (collision.gameObject.tag == "KillBox")
        {
            //Reloads the scene to the most recent checkpoint when the player hits a killbox
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Dead = true;
            
        }

    }




    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "KillBox")
        {
            //Reloads the scene to the most recent checkpoint when the player hits a killbox
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);'

            Dead = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {//Makes the game realize the player is not touching the ground
            print("Dont Touch Grass");
            // CoyoteTime = true;
            // IsColliding = false;
            Colliding--;


        }

    }

    // Update is called once per frame
    public void Update()
    {
       

        if (Dead == true)
        {

            PlayerRB.velocity = new Vector2(0, 0);
            PlayerRB.gravityScale = 0;
            OnDestroy();


        }

        if (PlayerShouldBeMoving == true)
        {
            
                moveDirection = move.ReadValue<float>();
            

        }

     
            if (Dead != true)
            {
                inputHorizontal = Input.GetAxis("Horizontal");
            }
        

        if (inputHorizontal > 0)
        {
            //Makes the player look right
            gameObject.transform.localScale = new Vector2(1, 1);
        }
        if (inputHorizontal < 0)
        {
            //Makes the player look left
            gameObject.transform.localScale = new Vector2(-1, 1);
        }
        if (inputHorizontal == 0)
        {//Makes the player go to idle animation
           
                Animator.SetBool("Moving", false);
              
        }
    }
    private void Handle_QuitPerformed(InputAction.CallbackContext onj)
    {//Quits the game when quit is pressed
        print("Handled quit Performed");
        QuitGame();
    }

    private void Handle_RestartPerformed(InputAction.CallbackContext obj)
    {//Sets the game back to the previous checkpoint
        print("Handled restart Performed");
        RestartGame();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
        print("Quit");
    }

}
