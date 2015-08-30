using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monty.ActiveRecord.Test.Base
{
    /// <summary>
    /// Base class for all tests entities
    /// </summary>
    /// <remarks>
    /// I would recomend that you aways implements a "Base" class for every new module that uses the Monty.ActiveRecord
    /// </remarks>
    /// <typeparam name="TMappedClass">The type of the mapped class.</typeparam>
    public class BaseTest<TMappedClass> : Monty.ActiveRecord.ActiveRecordBase<TMappedClass>
        where TMappedClass : class
    {
    }
}
