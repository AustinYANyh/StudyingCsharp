import matplotlib.pyplot as plt
import numpy as np
import random as random
import pandas as pd

"""
x=[2010,2011,2012,2013,2014,2015,2016,2017,2018]
y=[5,32,8,6,72,100,97,62,13]

plt.plot(x,y)
plt.show()
"""
"""
# random函数生成的是1-1000的随机数,返回值为一个数字
# numpy中的randn函数返回值为一个数组

# x=random.randint(1,1000)
# y=random.randint(1,1000)

x=np.random.randn(1000)
y=np.random.randn(1000)

plt.scatter(x,y,marker='x')
plt.show()
"""

a=np.random.randn(100);
s=pd.Series(a)

plt.hist(s)
plt.show()