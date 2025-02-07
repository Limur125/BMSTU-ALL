This is ApacheBench, Version 2.3 <$Revision: 1879490 $>
Copyright 1996 Adam Twiss, Zeus Technology Ltd, http://www.zeustech.net/
Licensed to The Apache Software Foundation, http://www.apache.org/

Benchmarking localhost (be patient).....done


Server Software:        Kestrel
Server Hostname:        localhost
Server Port:            80

Document Path:          /api/v1/games
Document Length:        2 bytes

Concurrency Level:      10
Time taken for tests:   2.880 seconds
Complete requests:      100
Failed requests:        0
Total transferred:      14100 bytes
HTML transferred:       200 bytes
Requests per second:    34.72 [#/sec] (mean)
Time per request:       288.007 [ms] (mean)
Time per request:       28.801 [ms] (mean, across all concurrent requests)
Transfer rate:          4.78 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    1   1.0      1       9
Processing:    13  141 342.3     19    1612
Waiting:       13  141 342.0     19    1609
Total:         13  142 342.5     20    1613

Percentage of the requests served within a certain time (ms)
  50%     20
  66%     22
  75%     31
  80%     43
  90%    947
  95%   1079
  98%   1104
  99%   1613
 100%   1613 (longest request)