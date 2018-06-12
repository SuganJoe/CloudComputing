using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace AWSLambda6
{
    class Utils
    {
        public CpuTime GetCpuUtilization()
        {
            String filename = "/proc/1/stat";

            if (File.Exists(filename))
            {
                string text = "";
                using (StreamReader sr = File.OpenText(filename))
                {
                    text = sr.ReadLine();
                }
                string[] par = text.Split(" ");
                return new CpuTime(Int64.Parse(par[13]),
                               Int64.Parse(par[14]),
                               Int64.Parse(par[15]),
                               Int64.Parse(par[16]));
            }
            else
            {
                return new CpuTime();
            }
        }
        

        public VmCpuStat getVmCpuStat()
        {
            string filename = "/proc/stat";

            if (File.Exists(filename))
            {
                string text = "";
                using (StreamReader sr = File.OpenText(filename))
                {
                    text = sr.ReadLine();
                
                    string [] par1 = text.Split(" ");
                    VmCpuStat vcs =  new VmCpuStat(Int64.Parse(par1[2]),
                                              Int64.Parse(par1[3]),
                                              Int64.Parse(par1[4]),
                                              Int64.Parse(par1[5]),
                                              Int64.Parse(par1[6]),
                                              Int64.Parse(par1[7]),
                                              Int64.Parse(par1[8]),
                                              Int64.Parse(par1[9]));


                    while ((text = sr.ReadLine()) != null && text.Length != 0)
                    {
                        // get boot time in ms since epoch
                        if (text.Contains("btime"))
                        {
                            par1 = text.Split(" ");
                            vcs.btime = Int64.Parse(par1[1]);
                        }
                    }

                    return vcs;
                }
           
            }
            else
                return new VmCpuStat();
        }

        public CpuTime getCpuTimeDiff(CpuTime c1, CpuTime c2)
        {
            return new CpuTime(c2.utime - c1.utime, c2.stime - c1.stime, c2.cutime - c1.cutime, c2.cstime - c1.cstime);
        }

        public VmCpuStat getVmCpuStatDiff(VmCpuStat v1, VmCpuStat v2)
        {
            return new VmCpuStat(v2.cpuusr - v1.cpuusr, v2.cpunice - v1.cpunice, v2.cpukrn - v1.cpukrn,
                                 v2.cpuidle - v1.cpuidle, v2.cpuiowait - v1.cpuiowait, v2.cpuirq - v1.cpuirq,
                                 v2.cpusirq - v1.cpusirq, v2.cpusteal - v1.cpusteal);
        }

        public class CpuTime
        {
            public long utime;
            public long stime;
            public long cutime;
            public long cstime;

            public CpuTime(long utime, long stime, long cutime, long cstime)
            {
                this.utime = utime;
                this.stime = stime;
                this.cutime = cutime;
                this.cstime = cstime;
            }

            public CpuTime()
            {
            }

            public String ToString()
            {
                return "utime=" + utime + " stime=" + stime + " cutime=" + cutime + " cstime=" + cstime + " ";
            }
        }

        public class VmCpuStat
        {
            public long cpuusr;
            public long cpunice;
            public long cpukrn;
            public long cpuidle;
            public long cpuiowait;
            public long cpuirq;
            public long cpusirq;
            public long cpusteal;
            public long btime;

            public VmCpuStat(long cpuusr, long cpunice, long cpukrn, long cpuidle,
                      long cpuiowait, long cpuirq, long cpusirq, long cpusteal)
            {
                this.cpuusr = cpuusr;
                this.cpunice = cpunice;
                this.cpukrn = cpukrn;
                this.cpuidle = cpuidle;
                this.cpuiowait = cpuiowait;
                this.cpuirq = cpuirq;
                this.cpusirq = cpusirq;
                this.cpusteal = cpusteal;
                //this.btime = btime;
            }
            public VmCpuStat()
            {
                
            }

            public String CpustatToString()
            {
                return "cpuusr=" + cpuusr + " cpunice=" + cpunice + " cpukrn=" + cpukrn + "cpuidle=" + cpuidle + "cpuiowait=" + cpuiowait +
                    " cpuirq=" + cpuirq + " cpusirq=" + cpusirq + "cpusteal=" + cpusteal + " ";
            }
        }
    }
}
