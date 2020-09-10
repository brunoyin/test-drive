from moviepy.editor import *
import os

"""
    iPhone photos have a 3 second smaller video. This is a great feature to help you remember the moment in motion.

    current version of moviepy is having trouble getting iPhone vertical mov right. A quick fix is to use ffmpeg to 
    convert mov to mp4:

    ```bash
    # convert MOV to mp4
    for x in `ls *.MOV`; do ffmpeg -i $x `basename $x .MOV`.mp4; done

    # support for Chinese font
    sudo apt install fonts-noto-cjk

    # audio
    youtube-dl -f 140 -k https://www.youtube.com/watch?v=ZbZSe6N_BXs -o Happy.m4a
    ```
"""

def test_mov1():
    W,H = 540,720
    clip = VideoFileClip('isaac/IMG_7797.mp4').set_position((0,-460))
    clip.save_frame('IMG_7797-test.jpg', 0)
    video = CompositeVideoClip([clip], size=(W,H))
    video.write_videofile("IMG_7797-test.mp4", fps=video.fps, ffmpeg_params=['-movflags','+faststart'])

def test_mov2():
    W,H = 540,720
    clip = VideoFileClip('isaac/IMG_4107.mp4').set_position((-930,-300))
    clip.save_frame('IMG_7797-test.jpg', 0)
    video = CompositeVideoClip([clip], size=(W,H))
    video.write_videofile("IMG_4107-test.mp4", fps=video.fps, ffmpeg_params=['-movflags','+faststart'])

def zoom(scale1=1, scale2=720/3024.0, ts=2 ):
    delta = scale2 - scale1
    def _zoom(t):
        t1 = min(t, ts)
        r = scale1 + t1/ts * delta
        return r
    return _zoom
    
def bounce_audio(x1, y1, h, a):
    """
        animation based on audio 
    """
    def set_position(t):
        pitches = a.get_frame(t)
        return (x1, int(y1 + pitches[-1] * h /2) )
    return set_position

def horizontal_clip():
    """
        Photo size: 4032x3024, MOV 1440 x 1080

        for vertical photos: scale only, crop needed for verrical display.

        Show still image with zooming out effect, them plays iPhone MOV taken with the photo
    """
    W,H = 540,720
    bg = ImageClip('isaac/IMG_8092.jpg', duration=5) # 4032x3024
    clip = VideoFileClip('isaac/IMG_8092.mp4', audio=False).resize(height=720).set_position((-210, 'center'))
    clip.save_frame('IMG_8092-test.jpg', 0)
    video = CompositeVideoClip([bg.set_position('center').resize(zoom()), clip.set_start(5-clip.duration)], size=(W,H))
    # video.write_videofile("IMG_8092-test.mp4", fps=video.fps, ffmpeg_params=['-movflags','+faststart'])
    return video

def vertical_group(files=['IMG_7796', 'IMG_7797','IMG_7798']):
    """
        Photo size: 3024 x 4032, MOV 1080 x 1440

        this is for multiple shots in very short period of time. It does not make any sense to show every still photos.
    """
    W,H = 540,720
    bg = ImageClip(f'isaac/{files[0]}.jpg', duration=5) # 4032x3024
    clips = []
    start_at = 2 
    for f in files:
        clip = VideoFileClip(f'isaac/{f}.mp4', audio=False).resize(height=720).set_position(('center', 'center')).set_start(start_at)
        # clip.save_frame(f'{f}-test.jpg', 0)
        clips.append(clip)
        start_at += clip.duration
    video = CompositeVideoClip([bg.set_position('center').resize(zoom(scale2=720/4032.0))] + clips, size=(W,H))
    # video.write_videofile(f"{f}-test.mp4", fps=video.fps, ffmpeg_params=['-movflags','+faststart'])
    return video

def vertical_clip(f='IMG_7982',duration=5):
    """
        Photo size: 3024 x 4032, MOV 1080 x 1440

        for vertical photos: scale only, no crop needed.

        Show still image with zooming out effect, them plays iPhone MOV taken with the photo
    """
    W,H = 540,720
    bg = ImageClip(f'isaac/{f}.jpg', duration=5) 
    clip = VideoFileClip(f'isaac/{f}.mp4', audio=False).resize(height=720).set_position(('center', 'center'))
    clip.save_frame(f'{f}-test.jpg', 0)
    video = CompositeVideoClip([bg.set_position('center').resize(zoom(scale2=720/4032.0)), clip.set_start(5-clip.duration)], size=(W,H))
    # video.write_videofile(f"{f}-test.mp4", fps=video.fps, ffmpeg_params=['-movflags','+faststart'])
    return video

def make_video():
    clips = [horizontal_clip(), vertical_group()]
    for x in ['IMG_7982', 'IMG_7988']:
        clips.append(vertical_clip(os.path.basename(x)))
    video = concatenate_videoclips(clips, method='compose')
    video.write_videofile("happy-issac.mp4", fps=video.fps, ffmpeg_params=['-movflags','+faststart'])


def main():
    W,H = 540,720 
    video = VideoFileClip('happy-issac.mp4', audio=False)
    audio = AudioFileClip('isaac/Happy.m4a').subclip(6,6+video.duration)
    video = video.set_audio(audio)
    # happy_face = VideoFileClip('isaac/happy-face.gif').resize(width=96).set_position(bounce_audio(200,520, 200,audio)) #, has_mask=True
    title = TextClip(
        u'业理',
        color='rgb(255, 128, 0)',
        font="Noto-Sans-CJK-SC-Bold",
        fontsize=30,
        kerning=3,
    ).set_duration(video.duration)
    final_video = CompositeVideoClip([video, title.set_position(bounce_audio((W-title.w)/2,620, 100,audio))], size=(W,H))
    final_video.write_videofile("happy-issac-final.mp4", fps=video.fps, ffmpeg_params=['-movflags','+faststart'])
if __name__ == '__main__':
    # test_mov3()
    # step 1:
    # make_video()
    # step 2: audio mix
    main()