using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingTable;

public class PotTable : BaseTable, IHasProgress
{
    public event EventHandler<IHasProgress.OnPregressChangedEventArgs> OnPregressChange;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Boiling,
        Boiled,
        Burned,
    }

    [SerializeField]
    private BoilingRecipeSO[] boilingRecipeSOArray;
    [SerializeField]
    private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float boilingTimer;
    private BoilingRecipeSO boilingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasIngredientObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Boiling:
                    boilingTimer += Time.deltaTime;

                    OnPregressChange?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                    {
                        progressNormalized = boilingTimer / boilingRecipeSO.boilingTimerMax
                    });

                    if (boilingTimer > boilingRecipeSO.boilingTimerMax)
                    {
                        GetIngredientObject().DestroySelf();

                        IngredientObject.SpawnIngredientObject(boilingRecipeSO.output, this);

                        state = State.Boiled;

                        burningTimer = 0f;

                        burningRecipeSO = GetBurningRecipeSOWithInput(GetIngredientObject().GetIngredientObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                    break;
                case State.Boiled:
                    burningTimer += Time.deltaTime;

                    OnPregressChange?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });

                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        GetIngredientObject().DestroySelf();

                        IngredientObject.SpawnIngredientObject(burningRecipeSO.output, this);

                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnPregressChange?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasIngredientObject())
        {
            if (player.HasIngredientObject())
            {
                if (HasRecipeWithInput(player.GetIngredientObject().GetIngredientObjectSO()))
                {
                    player.GetIngredientObject().SetIngredientObjectParent(this);

                    boilingRecipeSO = GetBoilingRecipeSOWithInput(GetIngredientObject().GetIngredientObjectSO());

                    state = State.Boiling;
                    boilingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnPregressChange?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                    {
                        progressNormalized = boilingTimer / boilingRecipeSO.boilingTimerMax
                    });
                }
            }
            else
            {
                //Player not carrying anything
            }
        }
        else
        {
            if (player.HasIngredientObject())
            {
                //Player is carrying something
            }
            else
            {
                GetIngredientObject().SetIngredientObjectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnPregressChange?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private bool HasRecipeWithInput(IngredientObjectSO inputIngredientObjectSO)
    {
        BoilingRecipeSO boilingRecipeSO = GetBoilingRecipeSOWithInput(inputIngredientObjectSO);
        return boilingRecipeSO != null;
    }

    private IngredientObjectSO GetOutputForInput(IngredientObjectSO inputIngredientObjectSO)
    {
        BoilingRecipeSO boilingRecipeSO = GetBoilingRecipeSOWithInput(inputIngredientObjectSO);
        if (boilingRecipeSO != null)
        {
            return boilingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private BoilingRecipeSO GetBoilingRecipeSOWithInput(IngredientObjectSO inputIngredientObjectSO)
    {
        foreach (BoilingRecipeSO boilingRecipeSO in boilingRecipeSOArray)
        {
            if (boilingRecipeSO.input == inputIngredientObjectSO)
            {
                return boilingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(IngredientObjectSO inputIngredientObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputIngredientObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
