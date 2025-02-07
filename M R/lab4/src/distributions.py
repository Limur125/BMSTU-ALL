import random
import numpy.random as mtrand
from scipy.stats import rv_continuous
import numpy as np

class EvenDistribution:
    def __init__(self, a: float, b: float):
        self.a = a
        self.b = b

    def generate(self):
        return self.a + (self.b - self.a) * random.random()


# class ErlangDistribution:
#     def __init__(self, k, alpha):
#         self.k = k
#         self.alpha = alpha

#     def generate(self):
#         return ss.erlang.rvs(self.k, self.alpha)
    
class HyperexpDistribution(rv_continuous):

    """An HyperExponential Random Variable
    """

    def __init__(self, alpha=0.5, lambda1=1.0, lambda2=1.0):
        self.alpha = alpha
        self.lambda1 = lambda1
        self.lambda2 = lambda2

    def generate(self, size=1):
        vsample = np.vectorize(self._single_sample)
        return np.fromfunction(vsample, (size,))

    def _single_sample(self, size):
        U1 = mtrand.random()
        if U1 <= self.alpha:
            scale = self.lambda1
        else:
            scale = self.lambda2
        U2 = mtrand.random()
        return -np.log(U2)/scale

    def pdf(self, x):
        a = self.alpha*self.lambda1*np.exp(self.lambda1*-x)
        b = (1-self.alpha)*self.lambda2*np.exp(self.lambda2*-x)
        return a + b

    def mean(self):
        return (self.alpha / self.lambda1) + ((1-self.alpha) / self.lambda2)

    def standard_dev(self):
        a = (self.alpha/(self.lambda1**2)) + ((1-self.alpha)/(self.lambda2**2))
        return np.sqrt(2*a + self.mean()**2)

    def cdf(self, x):
        a = self.alpha*(-np.exp(self.lambda1*-x))
        b = (1-self.alpha)*(-np.exp(self.lambda2*-x))
        return a + b + 1

    def CoV(self):
        a = np.sqrt(2*self.alpha/self.lambda1 + 2*(1-self.alpha)/self.lambda2 -
                    (self.alpha/self.lambda1 + (1-self.alpha)/self.lambda2)**2)
        return a/self.mean()
    