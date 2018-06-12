
using System;
using System.Collections.Generic;
using System.Text;

namespace FiboREST
{
    public class Responses
    {
        int output;
        long elapsedTime;
        public Responses()
        {

        }

        public Responses(int output)
        {
            this.output = output;
        }
        public Responses(int output, long elapsedTime)
        {

            this.output = output;
            this.elapsedTime = elapsedTime;

        }

        public String toString()
        {


            return ("output=" + output + ";" + "elapsedTime=" + elapsedTime.ToString() + "=");


        }
    }
}
