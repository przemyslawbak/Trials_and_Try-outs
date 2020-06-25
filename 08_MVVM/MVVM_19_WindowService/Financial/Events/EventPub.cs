using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Events
{
    using System;

    using System.Collections.Generic;

    using System.Linq;

    using System.Text;

    using System.Threading.Tasks;


    //Define publisher class as Pub

    public class Pub

    {

        //OnChange property containing all the

        //list of subscribers callback methods

        public Action OnChange { get; set; }


        public void Raise()

        {

            //Check if OnChange Action is set before invoking it

            if (OnChange != null)

            {

                //Invoke OnChange Action

                OnChange();

            }

        }

    }
}
