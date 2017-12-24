function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min)) + min;
}

export default class AudioProvider {
    constructor() {
        this.tracks = [0, 1, 2, 3].map(x => new Audio(`/audio/${x}.mp3`));
        this.current = 0;
    }
    play() {
        this.current = getRandomInt(0, this.tracks.length);
        this.tracks[this.current].play();
    }
    stop() {
        this.tracks[this.current].pause();
        this.tracks[this.current].currentTime = 0;
    }
}