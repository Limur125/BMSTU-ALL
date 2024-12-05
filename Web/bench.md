# Без балансировки

Server Software:        GameTime
Server Hostname:        localhost
Server Port:            80

Document Path:          /api/v1/games
Document Length:        166 bytes

Concurrency Level:      10
Time taken for tests:   8.412 seconds
Complete requests:      1000
Failed requests:        0
Total transferred:      141000 bytes
HTML transferred:       2000 bytes
Requests per second:    118.88 [#/sec] (mean)
Time per request:       84.117 [ms] (mean)
Time per request:       8.412 [ms] (mean, across all concurrent requests)
Transfer rate:          16.37 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    1   3.0      0      31
Processing:     9   72  50.0     89    1183
Waiting:        9   45  40.3     52    1143
Total:         11   73  49.6     90    1183

Percentage of the requests served within a certain time (ms)
  50%     90
  66%    101
  75%    104
  80%    105
  90%    108
  95%    113
  98%    119
  99%    123
 100%   1183 (longest request)

# Балансировка

Server Software:        GameTime
Server Hostname:        localhost
Server Port:            80

Document Path:          /api/v1/games
Document Length:        166 bytes

Concurrency Level:      10
Time taken for tests:   1.064 seconds
Complete requests:      1000
Failed requests:        0
Non-2xx responses:      1000
Total transferred:      329000 bytes
HTML transferred:       166000 bytes
Requests per second:    939.80 [#/sec] (mean)
Time per request:       10.641 [ms] (mean)
Time per request:       1.064 [ms] (mean, across all concurrent requests)
Transfer rate:          301.95 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    3   5.5      1      46
Processing:     0    7   8.3      3      59
Waiting:        0    5   7.6      2      47
Total:          3   10  10.5      5      77

Percentage of the requests served within a certain time (ms)
  50%      5
  66%      7
  75%     14
  80%     15
  90%     24
  95%     32
  98%     45
  99%     50
 100%     77 (longest request)