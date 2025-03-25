using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCustomerModelDebugger : MonoBehaviour
{
    [Header("Registered")]
    public List<SpecialCustomerModel> specialCustomers;
    void Awake()
    {
        specialCustomers = SpecialCustomerRegistry.SPECIAL_CUSTOMER_MODELS.Values.ToList();
    }
}
