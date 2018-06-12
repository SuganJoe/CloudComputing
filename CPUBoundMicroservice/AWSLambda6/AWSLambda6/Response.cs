using System;
using System.Collections.Generic;
using System.Text;

namespace AWSLambda6
{
    public class Response
    {
        String uuid;
        String error;
        long cpuUsr;
        long cpuKrn;
        long cutime;
        long cstime;
        int pid;
        long vmcpuusr;
        long vmcpunice;
        long vmcpukrn;
        long vmcpuidle;
        long vmcpuiowait;
        long vmcpuirq;
        long vmcpusirq;
        long vmcpusteal;
        long vmuptime;
        int newcontainer;
        int output;
        long elapsedTime;

        public String getUuid()
        {
            return uuid;
        }
        public void setUuid(String uuid)
        {
            this.uuid = uuid;
        }
        public long getCpuUsr()
        {
            return cpuUsr;
        }
        public void setCpuUsr(long cpuusr)
        {
            this.cpuUsr = cpuusr;
        }
        public long getCpuKrn()
        {
            return cpuKrn;
        }
        public void setCpuKrn(long cpukrn)
        {
            this.cpuKrn = cpukrn;
        }
        public long getCuTime()
        {
            return cutime;
        }
        public void setCuTime(long cutime)
        {
            this.cutime = cutime;
        }
        public long getCsTime()
        {
            return cstime;
        }
        public void setCsTime(long cstime)
        {
            this.cstime = cstime;
        }
        public String getError()
        {
            return error;
        }
        public void setError(String err)
        {
            this.error = err;
        }
        public int getPid()
        {
            return pid;
        }
        public void setPid(int pid)
        {
            this.pid = pid;
        }


        public long getVmcpuusr()
        {
            return vmcpuusr;
        }
        public void setVmCpuusr(long vmcpuusr)
        {
            this.vmcpuusr = vmcpuusr;
        }
        public long getVmcpunice()
        {
            return vmcpunice;
        }
        public void setVmcpunice(long vmcpunice)
        {
            this.vmcpunice = vmcpunice;
        }
        public long getVmcpukrn()
        {
            return vmcpukrn;
        }
        public void setVmcpukrn(long vmcpukrn)
        {
            this.vmcpukrn = vmcpukrn;
        }
        public long getVmcpuidle()
        {
            return vmcpuidle;
        }
        public void setVmcpuidle(long vmcpuidle)
        {
            this.vmcpuidle = vmcpuidle;
        }
        public long getVmcpuiowait()
        {
            return vmcpuiowait;
        }
        public void setVmcpuiowait(long vmcpuiowait)
        {
            this.vmcpuiowait = vmcpuiowait;
        }
        public long getVmcpuirq()
        {
            return vmcpuirq;
        }
        public void setVmcpuirq(long vmcpuirq)
        {
            this.vmcpuirq = vmcpuirq;
        }
        public long getVmcpusirq()
        {
            return vmcpusirq;
        }
        public void setVmcpusirq(long vmcpusirq)
        {
            this.vmcpusirq = vmcpusirq;
        }
        public long getVmcpusteal()
        {
            return vmcpusteal;
        }
        public void setVmcpusteal(long vmcpusteal)
        {
            this.vmcpusteal = vmcpusteal;
        }
        public long getVmuptime()
        {
            return this.vmuptime;
        }
        public void setVmuptime(long vmuptime)
        {
            this.vmuptime = vmuptime;
        }
        public int getNewcontainer()
        {
            return this.newcontainer;
        }
        public void setNewcontainer(int newcontainer)
        {
            this.newcontainer = newcontainer;
        }

        public Response(String uuid)
        {
            this.uuid = uuid;
        }
        public Response(String uuid, long cpuusr, long cpukrn)
        {
            this.uuid = uuid;
            this.cpuUsr = cpuusr;
            this.cpuKrn = cpukrn;
        }
        public Response(String uuid, long cpuusr, long cpukrn, long cutime, long cstime)
        {
            this.uuid = uuid;
            this.cpuUsr = cpuusr;
            this.cpuKrn = cpukrn;
            this.cutime = cutime;
            this.cstime = cstime;
        }
        public Response(String uuid, long cpuusr, long cpukrn, long cutime, long cstime,
                        long vmcpuusr, long vmcpunice, long vmcpukrn, long vmcpuidle, long vmcpuiowait,
                        long vmcpuirq, long vmcpusirq, long vmcpusteal, long vuptime, int newcontainer, int output)
        {
            this.uuid = uuid;
            this.cpuUsr = cpuusr;
            this.cpuKrn = cpukrn;
            this.cutime = cutime;
            this.cstime = cstime;
            this.vmcpuusr = vmcpuusr;
            this.vmcpunice = vmcpunice;
            this.vmcpukrn = vmcpukrn;
            this.vmcpuidle = vmcpuidle;
            this.vmcpuiowait = vmcpuiowait;
            this.vmcpuirq = vmcpuirq;
            this.vmcpusirq = vmcpusirq;
            this.vmcpusteal = vmcpusteal;
            this.vmuptime = vuptime;
            this.newcontainer = newcontainer;
            this.output = output;
        }


        public Response()
        {

        }

        public Response(int output)
        {
            this.output = output;
        }

        //public String toString()
        //{
        //    /*
        //    return "uuid=" + this.getUuid() + " cpuusr=" + this.getCpuUsr() + " cpukrn=" + this.getCpuKrn()
        //            + " cutime=" + this.getCuTime() + " cstime=" + this.getCsTime()
        //            + " vmuptime=" + this.getVmuptime() + " vmcpusteal=" + this.getVmcpusteal() + " vmcpuusr="
        //            + this.getVmcpuusr() + " vmcpukrn=" + this.getVmcpukrn() + " vmcpuidle=" + this.getVmcpuidle();
        //            */

        //    return "output= " + output;


        //}

        public Response(int output, long elapsedTime)
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
