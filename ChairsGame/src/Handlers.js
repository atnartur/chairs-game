export default class Handlers {
    constructor(game) {
        const ws = game.ws;
        ws.on({
            user_logged_count: data => {
                game.preview.playersCount = data.count;
                if (game.preview.playersCount > 1 && this.isFirst)
                    game.preview.isStartButtonHide = false;
            },
            user_logged_in: data => game.messages.push(`${data.username} вошел(а)`),
            user_is_first: data => this.isFirst = data.is_first,
            startGame: data => {
                game.preview.isHide = true;
                game.chairs.setPositions(data.countOfChairs);
                game.chairs.isHide = false;
                game.audio.play();
            },
            musicStop: () => game.audio.stop(),
            // click: () => game.chairs[]
            close: () => {
                if (confirm('Произошла ошибка подключения. Перезапустить игру?'))
                    location.reload();
            }
        });
    }
}
