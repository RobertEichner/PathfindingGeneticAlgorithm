using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAgent : MonoBehaviour
{
   //Creature Characteristics
   private Creature creature = null;
   private int genAmount;
   private float speed;
   private float maxTargetDistance;
   private float stepLength;

   //Pathfidning
   private bool hasFinished = true;
   private bool hasCollided = false;
   private Vector2 targetPosition = Vector2.zero;
   private int pathIndex = 0;
   
   //Helper
   private Rigidbody2D rb = null;
   [SerializeField] private LayerMask obstacleLayer;
   private float obstaclePenalty = 1.0f;

   public float ObstaclePenalty
   {
      get => obstaclePenalty;
      set => obstaclePenalty = value;
   }

   public Creature Creature
   {
      get => creature;
      set => creature = value;
   }

   public Rigidbody2D Rb => rb;
   public bool HasFinished => hasFinished;

   public void StartAgent()
   {
      TryGetComponent(out rb);
      creature = new Creature(genAmount);
      ResetCreature();
   }

   private void FixedUpdate()
   {
      if (!hasFinished)
         FindPath();
   }

   public void SetAgentValues(int genAmount, float speed, float maxTargetDistance, float stepLength)
   {
      this.genAmount = genAmount;
      this.speed = speed;
      this.maxTargetDistance = maxTargetDistance;
      this.stepLength = stepLength;
   }

  private void FindPath()
   {
      if (Vector2.Distance(rb.position,targetPosition) <= maxTargetDistance)
      {
         pathIndex++;
         if (pathIndex >= creature.GenPath.Count)
            hasFinished = true;
         else
            targetPosition = rb.position + creature.GenPath[pathIndex];
      }
      else
      {
         rb.MovePosition(rb.position + creature.GenPath[pathIndex] * (Time.deltaTime * speed * stepLength));
      }
   }

  private void OnCollisionEnter2D(Collision2D other)
  {
     if (other.collider.CompareTag("Wall"))
     {
        hasFinished = true;
        hasCollided = true;
     }
     
  }

  public float GetFitness(Vector3 targetPos)
  {
     float distance = Vector3.Distance(rb.position, targetPos);
     if (hasCollided &&  distance > 0.0125)
        return distance * obstaclePenalty;
     return distance;
  }

  public void ResetCreature()
   {
      hasFinished = false;
      hasCollided = false;
      targetPosition = rb.position + creature.GenPath[0];
      pathIndex = 0;
   }
   
   public void ResetCreature(Vector2 resetPosition)
   {
      rb.position = resetPosition;
      ResetCreature();
     
   }
   
}
