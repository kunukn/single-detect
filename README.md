Single Detect with C#
=============

Detect singles and K-nearest neighbors in 2D dimension space
* Console code usage example
* Gui code usage example
* Algorithm strategy option between Grid, Naive and KdTree

<br>

Single detection
------------

About Single detection? Read this [blog](http://kunuk.wordpress.com/2013/01/13/single-detection-in-2d-dimension).

![Single Detect](https://raw.github.com/kunukn/single-detect/master/img/singledetect.gif "single detect image")

The red colors are the identified singles.

<br>

K-nearest neighbor
------------

About K-nearest neighbor? Read this [blog](http://kunuk.wordpress.com/2013/01/21/k-nearest-neighbor-in-2d-dimension-space).

![K Nearest Neighbor](https://raw.github.com/kunukn/single-detect/master/img/knn.gif "knn image")

The origin in the center is the red color. <br>
The pink colors are the nearest neighbors and are slightly larger in size.

![K Nearest Neighbor](https://raw.github.com/kunukn/single-detect/master/img/knn2.gif "knn image2")

Nearest neighbors by same type only. The origin is the red and the same type are all the purple colors. <br>
The identified nearest neighbors are slightly larger in size (Only supported in Naive and Grid strategy algorithm).
