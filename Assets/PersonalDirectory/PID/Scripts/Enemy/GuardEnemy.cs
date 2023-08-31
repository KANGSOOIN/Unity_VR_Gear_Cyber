using ildoo;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
namespace PID
{
    [RequireComponent(typeof(NavMeshAgent), typeof(SightFunction))]
    public class GuardEnemy : BaseEnemy
    {
        public enum State
        {
            Idle,
            Patrol,
            LookAround, 
            Alert,
            Assault,
            Trace,
            Neutralized,
            Size
            //Possibly extending beyond for Gathering Abilities. 
        }
        //Base Properties 
        Animator anim;
        Rigidbody rigid;
        NavMeshAgent agent;
        SightFunction guardSight;
        StateMachine<State, GuardEnemy> stateMachine;
        [SerializeField] Transform robotBody; 
        public Transform RobotBody => robotBody;
        [SerializeField] Transform[] patrolPoints; 
        bool notified;

        //Combat Extra Properties
        [SerializeField] float fireInterval_s;
        [SerializeField] float randomShotRadius;
        WaitForSeconds fireInterval;

        protected void Awake()
        {
            enemyStat = GameManager.Resource.Instantiate<EnemyStat>("Data/Guard");
            notified = false;

            base.SetUp(enemyStat);
            stateMachine = new StateMachine<State, GuardEnemy>(this);
            fireInterval = new WaitForSeconds(fireInterval_s);
            stateMachine.AddState(State.Idle, new IdleState(this, stateMachine));
            stateMachine.AddState(State.Patrol, new PatrolState(this, stateMachine));
            stateMachine.AddState(State.Alert, new AlertState(this, stateMachine));
            stateMachine.AddState(State.Assault, new AssaultState(this, stateMachine));
            stateMachine.AddState(State.Neutralized, new NeutralizedState(this, stateMachine));
        }
        #region MACHINE RUNNING 
        private void Start()
        {
            stateMachine.SetUp(State.Idle);
        }
        private void Update()
        {
            stateMachine.Update();
        }

        protected override void Die()
        {
            NeutralizedState neutralizeReason;
            if (stateMachine.CheckState(State.Neutralized))
            {
                neutralizeReason = stateMachine.RetrieveState(State.Neutralized) as NeutralizedState;
            }
            stateMachine.ChangeState(State.Neutralized);
        }
        #endregion

        #region  Combat Interaction 
        Coroutine fireCoroutine; 
        public void Fire()
        {
            fireCoroutine = StartCoroutine(FireRoutine());
        }

        public void StopFire()
        {

        }
        IEnumerator FireRoutine()
        {
            while (true)
            {
                yield return fireInterval;
            }
        }



        public override void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            base.TakeDamage(damage, hitPoint, hitNormal);
            if (currentHealth <= 0)
            {
                NeutralizedState neutralizeReason;
                if (stateMachine.CheckState(State.Neutralized))
                {
                    neutralizeReason = stateMachine.RetrieveState(State.Neutralized) as NeutralizedState;
                    neutralizeReason.SetDeathReason(NeutralizedState.DeathType.Health); 
                }
                stateMachine.ChangeState(State.Neutralized);
            }
        }

        public void Notified(int index, Vector3 centrePoint)
        {
            if (stateMachine.curStateName == State.Neutralized)
            {
                return;
            }
            AlertState alertState;
            if (stateMachine.CheckState(State.Alert))
            {
                alertState = stateMachine.RetrieveState(State.Alert) as AlertState;
                alertState.SetDestination(centrePoint);
            }
        }

        public void Reactivate()
        {
            NeutralizedState.DeathType deathState; 
            if (stateMachine.curStateName != State.Neutralized)
                return;
            //Run the reason to see if the robot was destroyed or hacked. 
            NeutralizedState neutralizeReason;
            if (stateMachine.CheckState(State.Neutralized))
            {
                neutralizeReason = stateMachine.RetrieveState(State.Neutralized) as NeutralizedState;
                deathState = neutralizeReason.DeathReason; 
                if (deathState == NeutralizedState.DeathType.Hacked)
                {
                    //Reactivate; => Should add a state where it is now looking for the vengence. 
                    stateMachine.ChangeState(State.Idle); 
                }
            }

        }

        //Hackable Component. 
        #endregion

        public abstract class GuardState : StateBase<State, GuardEnemy>
        {
            public Animator anim => owner.anim;
            public GuardState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }
        }

        #region Idle State 
        public class IdleState : GuardState
        {
            public IdleState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                //Game Started, Robots are Initialized. 
                // Depending on the Start Up notion, this should determine type of States this should switch to. 
            }

            public override void Exit()
            {
            }

            public override void Setup()
            {
            }

            public override void Transition()
            {
            }

            public override void Update()
            {
            }
        }
        #endregion

        #region Patrol State 
        public class PatrolState : GuardState
        {
            int lastPatrolPoint;
            int nextPatrolPoint;
            int patrolCount;
            Vector3 patrolDestination; 
            PriorityQueue<DestinationPoint> patrolQueue; 
            public PatrolState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                //Compute to find nearest patrol starting point based on robot's current position, get the nearest 
            }

            public override void Exit()
            {
                patrolCount = 0; 
            }

            public override void Setup()
            {
                lastPatrolPoint = 0;
                patrolQueue = new PriorityQueue<DestinationPoint> ();
                for (int i = 0; i < owner.patrolPoints.Length; i++)
                {
                    float tempDist = Vector3.SqrMagnitude(owner.patrolPoints[i].position - owner.transform.position);
                    DestinationPoint tempPoint = new DestinationPoint(owner.patrolPoints[i].position, tempDist); 
                    patrolQueue.Enqueue(tempPoint);
                }
            }

            public override void Transition()
            {
                if (patrolCount >= owner.patrolPoints.Length)
                {

                }

            }

            public override void Update()
            {
                throw new System.NotImplementedException();
            }
        }
        #endregion

        #region LookAround State 
        public class LookAroundState : GuardState
        {
            public const float lookAroundTime = 5f;
            Quaternion previousRotation;
            Quaternion searchRotation; 
            public float timer; 
            public LookAroundState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                owner.agent.isStopped = true; 
                Vector3 nextLookDir = UnityEngine.Random.insideUnitCircle.normalized; 
                previousRotation = owner.transform.rotation;
                searchRotation = Quaternion.LookRotation(nextLookDir, owner.transform.up); 
                //TODO: Add Method to check if the next search rotation is too close to current rotation; 
            }

            public override void Exit()
            {
                timer = 0f;
                owner.agent.isStopped = false; 
            }

            public override void Setup()
            {
                //Set Next Random Pointer to Look At, 
            }

            public override void Transition()
            {
                if (timer >= lookAroundTime)
                    stateMachine.ChangeState(State.Patrol); 
            }

            public override void Update()
            {

            }
        }
        #endregion
        #region Alert State
        //Activated when CCTV detects a Player, 
        // Main Function being, Each assigned Guard should head back to the designated CCTV regions. 
        public class AlertState : GuardState
        {
            SightFunction guardSight;
            Vector3 destPoint;
            float distDelta;
            const float distThreshhold = 3.5f;
            public AlertState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                throw new System.NotImplementedException();
            }

            public override void Exit()
            {
                owner.notified = false;
            }

            public override void Setup()
            {
                guardSight = owner.guardSight;
            }

            public void SetDestination(Vector3 location)
            {
                destPoint = location;
            }

            public override void Transition()
            {
                switch (guardSight.TargetFound)
                {
                    case true:
                        break;
                    case false:
                        stateMachine.ChangeState(State.Assault);
                        break;
                }
                if (guardSight.TargetFound)
                {

                }
                else if (!guardSight.TargetFound)
                {

                }
            }

            public override void Update()
            {
                if (distDelta > distThreshhold)
                    return;
            }
        }
        #endregion

        #region Assault 
        public class AssaultState : GuardState
        {
            public AssaultState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                owner.agent.isStopped = true;
                
            }

            public override void Exit()
            {
                owner.agent.isStopped = false;
                //Perhaps for now, but should consider reasons for movement 
            }

            public override void Setup()
            {
                throw new System.NotImplementedException();
            }

            public override void Transition()
            {

            }

            public override void Update()
            {
            }
        }
        #endregion

        #region Trace State 
        // Given an Enemy previously 'Founnd' the enemy, but temporaily have the player gone missing, for a certain interval, Enemy will Search for the Player Under 2 Conditions. 
        // 1. Timer Runs out => Returns to the Patrol or Look Around State
        // 2. Player Dies. 
        public class TraceState : GuardState
        {
            const float maxTraceTime = 3f;
            float trackTimer;

            public TraceState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                trackTimer = 0f;
            }

            public override void Exit()
            {
                trackTimer = 0f;
            }

            public override void Setup()
            {
                throw new System.NotImplementedException();
            }

            public override void Transition()
            {
                if (trackTimer >= maxTraceTime)
                {
                    stateMachine.ChangeState(State.LookAround);
                    return;
                }
            }

            public override void Update()
            {
                trackTimer += Time.deltaTime;
            }
        }
        #endregion

        #region Neutralized 
        public class NeutralizedState : GuardState
        {
            public enum DeathType
            {
                Health,
                Hacked,
                None
            }
            DeathType deathReason;
            public DeathType DeathReason => deathReason; 
            public NeutralizedState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public void SetDeathReason(DeathType type)
            {
                deathReason = type;
            }
            public override void Enter()
            {
                //Should Notify CCTV to erase from the list 
                owner.anim.SetTrigger("");
            }
            public override void Exit()
            {
                owner.anim.Rebind();
                deathReason = DeathType.None;
            }
            public override void Setup()
            {
                deathReason = DeathType.None; 
                //Should consider placing in other effects for further interactable elements. 
            }
            public override void Transition()
            { }
            public override void Update()
            { }
        }
        #endregion
    }
}