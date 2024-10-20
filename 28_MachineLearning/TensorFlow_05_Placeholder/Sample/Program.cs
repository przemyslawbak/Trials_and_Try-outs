using System;
using Tensorflow;
using static Tensorflow.Binding;

namespace Sample
{
    //https://tensorflownet.readthedocs.io/en/latest/Placeholder.html
    class Program
    {
        static void Main(string[] args)
        {
            tf.compat.v1.disable_eager_execution();
            var x = tf.placeholder(tf.int32);
            var y = x * 3;

            using (var sess = tf.Session())
            {
                var result = sess.run(y, feed_dict: new FeedItem[]
                {
                    new FeedItem(x, 2)
                });
                // (int)result should be 6;
            }
        }
    }
}