﻿/*  RELEASE NOTE
 *  Copyright (C) 2018 BIRENCHENS
 *  All right reserved
 *
 *  Filename:       General_Model.cs
 *  Desctiption:    
 *
 *  CreateBy:       BIRENCHENS
 *  CreateDate:     2019-01-15 08:49:35
 *
 *  Version:        V1.0.0
 ***********************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBUtility;

namespace Builder.Constructer.Models
{
    public class General_Model : IConstructible
    {
        private readonly DataSet ds;

        public General_Model()
        {

        }

        /// <summary>
        /// Model 建造
        /// </summary>
        /// <returns></returns>
        public StringBuilder Construct()
        {
            throw new NotImplementedException();
        }
    }
}
