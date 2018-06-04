import sys;
import json;

totalcount=0;
avgmemory=0;
totalusage=0;

jsondata=json.load(sys.stdin);
functioncount=jsondata['value'][0]['timeseries'][0]['data'];
functionusage=jsondata['value'][1]['timeseries'][0]['data'];
functionmemory=jsondata['value'][2]['timeseries'][0]['data'];

for c in functioncount:
    totalcount += c['total'];

for u in functionusage:
    totalusage += u['total'];

for m in functionmemory:
    avgmemory += m['total'];

print("Total runs: {} ".format(totalcount));
print("Total memory used: {} MB".format((avgmemory*totalcount)/(1024*1024)));
print("Avg memory used: {} MB".format(avgmemory/(1024*1024)));
print("Cost: {} GB-s".format(totalusage/(1024*1000.0)));
