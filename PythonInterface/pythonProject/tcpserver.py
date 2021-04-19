import socket
import time
import struct
import random
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind(('127.0.0.1', 7788))
s.listen()
conn, addr = s.accept()
print("connected by", addr)
send_list = [[0.,0.,0.,5.],
             [10.,0.,0.,5.],
             [10.,0.,10.,5.],
             [0.,0.,10.,5.]]
iter = 0

foot_list = [[0.,0.,0.,-10.],
             [0.,0.,0.,10.],
             [0.,0.,0.,-10],
             [0.,0.,0.,10.]]

body_tail_list = [[10.,0.,0.,0.,0.,0.],[0.,0.,10.,10.,0.,10.],[10.,10.,0.,0.,10.,0.]]

fixed_list = [0.,0.,0.,float(random.randint(0,15))]
# while True:
while True:
    #r_list = [float(random.randint(0,15)), 1.,float(random.randint(0,15))]
    #fixed_list = [0., 0., 0., float(random.randint(0, 15))]
    idx = iter % 3
    time.sleep(0.5)
    buf = b''
    for i in body_tail_list[idx]:
        buf += struct.pack('<f', i)
    conn.sendall(buf)
    iter += 1