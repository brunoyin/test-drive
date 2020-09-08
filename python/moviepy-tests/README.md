## moviepy: Python lib for video editing

[https://zulko.github.io/moviepy/](https://zulko.github.io/moviepy/)

moviepy automates [ffmpeg](https://ffmpeg.org/) commands and allows frame by frame video editing 

### installation

```bash
# Requirements on Ubuntux Linux
# run as root
su -
# Install numpy using system package manager
apt-get -y update && apt-get -y install ffmpeg imagemagick

# Install some special fonts we use in testing, etc..
apt-get -y install fonts-liberation

# imagemagick policy fix
cat /etc/ImageMagick-6/policy.xml | sed 's/none/read,write/g'> /etc/ImageMagick-6/policy.xml 

# you may not need this
apt-get install -y locales && \
    locale-gen C.UTF-8 && \
    /usr/sbin/update-locale LANG=C.UTF-8

ENV LC_ALL C.UTF-8

# install
pip install moviepy
# 
# Install Scipy, PIL, Pillow or OpenCV for additional image manipulation

```