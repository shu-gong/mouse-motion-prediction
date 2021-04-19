using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MousePlayground
{
    public class MouseController : MonoBehaviour
    {
        public Transform mainBody;
        public Transform hindJointL;
        public Transform hindFootL;
        public Transform hindJointR;
        public Transform hindFootR;
        public Transform foreJointL;
        public Transform foreFootL;
        public Transform foreJointR;
        public Transform foreFootR;
        public Transform nose;
        public Transform tail5;
        public Transform tail9;
        public int bytes_per_chunk = 144;
        EndPoint point;
        byte[] data;
        Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        void Awake()
        {
            IPAddress ipaddress = IPAddress.Parse("127.0.0.1");
            EndPoint point = new IPEndPoint(ipaddress, 7788);
            tcpClient.Connect(point); 
        }

        void FixedUpdate()
        {

            float speed = 1;
            if (tcpClient.Available > 0)
            {
                byte[] data = new byte[1024];

                int data_length = tcpClient.Receive(data);

                if (data_length > 0 & data_length % bytes_per_chunk == 0)
                {
                    int chunks_to_read = data_length / bytes_per_chunk;
                    
                    int index = 0;
                    /*
                    Transform[] body_part_array =
                        new Transform[2]
                        {
                            mainBody, tail9
                        };
                    */
                    Transform[] body_part_array =
                        new Transform[12]
                        {
                            mainBody, nose, tail5, tail9,
                            foreJointL, foreFootL,
                            foreJointR, foreFootR,
                            hindJointL, hindFootL,
                            hindJointR, hindFootR
                        };
                    
                    for (int i = 0; i < chunks_to_read; i++)
                    {
                        foreach (Transform body_part in body_part_array)
                        {
                            if (body_part == mainBody)
                            {
                                Tuple<int, Vector3> Tupler = readFloatCoordinate(data, index);
                                body_part.rotation =
                                    Quaternion.LookRotation(Tupler.Item2 - body_part.position, Vector3.up);
                                body_part.position = Tupler.Item2;
                                index = Tupler.Item1;
                            }
                            else
                            {
                                Tuple<int, Vector3> Tupler = readFloatCoordinate(data, index);
                                body_part.position = Tupler.Item2;
                                index = Tupler.Item1;
                            }
                        }
                    }
                }
            }
        }
        
        Tuple<int, Vector3> readFloatCoordinate(byte[] byte_data, int idx)
        {
            float ret_x = System.BitConverter.ToSingle(byte_data, idx);
            float ret_y = System.BitConverter.ToSingle(byte_data, idx + 4);
            float ret_z = System.BitConverter.ToSingle(byte_data, idx + 8);
            Vector3 ret_vector = new Vector3(ret_x, ret_y, ret_z);
            int ret_idx = idx + 12;
            Tuple<int, Vector3> ret_tuple = new Tuple<int, Vector3>(ret_idx, ret_vector);
            return ret_tuple;
        }
    }
}