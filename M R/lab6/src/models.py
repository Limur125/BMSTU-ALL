import random

import distributions as d
import myqueue as q
import request as r

class EventModel:

    def __init__(self, request_generator, oa_generator, return_probability, limit=10000, iters=1000):
        self.req_gen = request_generator
        self.oa_gen = oa_generator
        self.limit = limit
        self.iters = iters
        self.ret_prob = return_probability

    def event(self):
        arr = q.MyQueue(self.limit)
        i = 0
        tmp_id = 0
        all_time = 0
        time_to_new_request = 0
        time_to_handle = 100
        max_len = 0
        cur_len = 0
        is_occuped = False
        while i < self.iters:
            if time_to_new_request < time_to_handle:
                all_time += time_to_new_request
                new_req = r.Request(tmp_id)
                tmp_id += 1
                arr.enqueue(new_req)
                cur_len += 1
                if not is_occuped:
                    time_to_handle = self.oa_gen.generate()
                    is_occuped = True
                else:
                    time_to_handle -= time_to_new_request
                time_to_new_request = self.req_gen.generate()
            else:
                if is_occuped:
                    all_time += time_to_handle
                    time_to_new_request -= time_to_handle
                    prob_to_return = random.random()
                    if prob_to_return < self.ret_prob:
                        arr.enqueue(arr.dequeue())
                    else:
                        cur_len -= 1
                        if cur_len == 0:
                            is_occuped = False
                    time_to_handle = self.oa_gen.generate()
                    i += 1
                else:
                    all_time += time_to_new_request
                    time_to_new_request = 0
            if cur_len > max_len:
                max_len = cur_len

        return max_len

class TimeModel:

    def __init__(self, request_generator, oa_generator, delta, return_probability, limit=10000, iters=1000):
        self.req_gen = request_generator
        self.oa_gen = oa_generator
        self.delta = delta
        self.limit = limit
        self.iters = iters
        self.ret_prob = return_probability

    def event(self):
        arr = q.MyQueue(self.limit)
        i = 0
        tmp_id = 0
        all_time = 0
        time_to_new_request = self.req_gen.generate()
        time_to_handle = self.oa_gen.generate()[0]
        max_len = 0
        cur_len = 0
        is_occuped = False
        while i < self.iters:
            time_to_new_request -= self.delta
            time_to_handle -= self.delta
            all_time += self.delta
            if time_to_new_request <= 0:
                new_req = r.Request(tmp_id)
                tmp_id += 1
                arr.enqueue(new_req)
                time_to_new_request = self.req_gen.generate()
                cur_len += 1
                if not is_occuped:
                    time_to_handle = self.oa_gen.generate()[0]
                    is_occuped = True
            if cur_len != 0 and time_to_handle <= 0:
                time_to_handle = self.oa_gen.generate()[0]
                if is_occuped:
                    prob_to_return = random.random()
                    if prob_to_return < self.ret_prob:
                        arr.enqueue(arr.dequeue())
                    else:
                        cur_len -= 1
                        if cur_len == 0:
                            is_occuped = False
                    i += 1
            if cur_len > max_len:
                max_len = cur_len

        return max_len