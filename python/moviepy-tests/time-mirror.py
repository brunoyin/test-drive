from moviepy.editor import *

'''
    Creating a loop video = [clip1 = 4 second clip] + [clip2 = clip1 in reverse]
'''


def create_loop():
    """
        create 16 second video using a 4 second video + reverse loop
    """
    W,H = 720,1280
    # find the video cut, resize, and do not load audio, the audio does not make sense in reverse
    # and moviepy does not work properly in dealing reverse audio
    clip = VideoFileClip('isaac/IMG_8057.mp4', audio=False).resize(width=W).subclip(0,4)
    # create the second clip in reverse
    clip2 = vfx.time_mirror(clip)
    # combine them, and loop one more time
    video = vfx.loop(concatenate_videoclips([clip, clip2], method='compose'), duration=4*4)
    # ffmpeg_params is needed for playing on Android
    video.write_videofile("isaac-loopx4.mp4", fps=clip.fps, ffmpeg_params=['-movflags','+faststart'])

# creating 1, 2, 3 zooming effect using resize scale
def zoom(v, scale_start=0.5, scale_end=5):
    w,h = v.size
    scale_delta = scale_end - scale_start
    
    def _zoom(t):
        scale = scale_start + scale_delta * (t / v.duration)
        return scale
    return _zoom


def main():
    # create_loop()
    W,H=544,960
    clip = vfx.loop(VideoFileClip('isaac-loopx4.mp4', audio=False).crop(x1=150, y1=115, x2=150+W, y2=115+H), duration=16*2)
    # match audio length with clip length
    audio = AudioFileClip('baby-dance.mp4').subclip(0,clip.duration)
    # create 1,2,3 intro: the digit will be displayed 0.5 second, show up in 1 second apart
    titles = []
    for i, x in enumerate(['1', '2', '3']):
        txt = TextClip(
            x,
            color='white',
            font="Noto-Sans-CJK-SC-Bold",
            fontsize=160,
            kerning=3,
        ).set_position('center').set_duration(0.5).set_start(i)
        txt = txt.resize(zoom(txt))
        titles.append(txt)
    # we need the first frame as the background image for the intro
    intro_bg = ImageClip(clip.get_frame(0), duration=3.5)
    # Intro is a composite video: clips are stacked, you need to specify size
    intro = CompositeVideoClip([intro_bg] + titles, size=(W,H))
    # concatenate_videoclips will combine all clips into one: they must have the same exact size
    video = concatenate_videoclips([intro, clip.set_audio(audio)], method='compose')
    # write to file: ffmpeg_params is needed for playing on Android
    video.write_videofile("isaac-anim2.mp4", fps=clip.fps, ffmpeg_params=['-movflags','+faststart'])

if __name__ == '__main__':
    # create_loop()
    main()
    