import sys;
import json;
import re;

memory=0;
duration=0;
events=json.load(sys.stdin)['events'];

for e in events:
    str1 = e['message'].split("Max Memory Used: ")[1];
    m = re.findall('\d+', str1);
    memory += float(m[0]);

    str2 = e['message'].split("Billed Duration: ")[1];
    d = re.findall('\d+', str2);
    duration += int(d[0]);

print("Total runs: {}".format(len(events)));
print("Total Memory used: {} MB".format(memory));
print("Avg Memory used: {} MB".format(memory/len(events)));
print("Total billed duration: {} s".format(duration/1000.0));
print("Cost: {} GB-s".format((256.0*duration)/(1024*1000)));
