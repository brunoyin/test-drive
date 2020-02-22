#
import requests
from requests.auth import HTTPBasicAuth

run_url = 'http://192.168.1.250:9876/run'
auth = HTTPBasicAuth("folaris", "folaris")
cmd = {"cmd": 'Get-Date'}
#
def run_folaris():
    ret = requests.post(run_url, json=cmd, auth=auth)

total = 1000

if __name__ == '__main__':
    import sys, time
    total = 1000
    if len(sys.argv) > 1:
        total = int(sys.argv[1])
    t1 = time.time()
    for x in range(0, total):
        run_folaris()
    ts = time.time() - t1
    avg = total / ts
    message = f"Total {total} calls done in {ts} seconds, average {avg} calls per second"
    print(message)
