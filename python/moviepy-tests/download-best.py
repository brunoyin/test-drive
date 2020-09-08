# -f -o

import sys
import youtube_dl

def download(url, filename):
    ydl_opts = {
        'format': "bestvideo[ext=mp4]+bestaudio[ext=m4a]/best[ext=mp4]/best" ,
        'outtmpl': f'{filename}-%(title)s.%(ext)s',
        'merge_output_format': 'mp4'
    }
    with youtube_dl.YoutubeDL(ydl_opts) as ydl:
        ydl.download([url])

if __name__ == '__main__':
    if len(sys.argv) != 3:
        print('need url and output file name as first, second arguments')
        sys.exit(1)
    download(sys.argv[1], sys.argv[2])