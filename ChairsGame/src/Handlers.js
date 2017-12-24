export default class Handlers {
    constructor(game) {
        let clickedOnChair = false;
        let musicStopped = false;
        
        function finish() {
            game.chairs.isHide = true;
            game.showFinal = true;
        }
        
        const ws = game.ws;
        
        ws.on({
            user_logged_count: data => {
                game.preview.playersCount = data.count;
                
                if (game.preview.playersCount > 1) 
                    game.preview.text = 'Первый подключившийся игрок должен начать игру';
                
                if (game.preview.playersCount > 1 && this.isFirst) {
                    game.preview.isStartButtonHide = false;
                    game.preview.text = 'Нажмите на кнопку, чтобы начать игру с таким количеством игроков'
                }
            },
            wait: data => game.preview.text = 'Ждем завершения другой игры', 
            user_logged_in: data => game.messages.push(`${data.username} вошел(а)`),
            user_is_first: data => this.isFirst = data.is_first,
            startGame: data => {
                clickedOnChair = false;
                game.preview.isHide = true;
                game.audio.play();
                game.chairs.isHide = false;
                setTimeout(() => game.chairs.setPositions(data.countOfChairs));
            },
            musicStop: () => {
                game.audio.stop()
                musicStopped = true
            },
            clickedOnChair: data => game.chairs[data.numberOfChair].isClicked = true,
            kick: () => {
                game.finalText = 'Вы проиграли!';
                finish();
            },
            win: () => {
                game.finalText = 'Вы выиграли!';
                finish();
            },
            close: () => {
                if (confirm('Произошла ошибка подключения. Перезапустить игру?'))
                    location.reload();
            }
        });
        
        game.chairs.on('click', numberOfChair => {
            if (!clickedOnChair && musicStopped) {
                ws.send('click', {numberOfChair});
                game.chairs[numberOfChair].isClicked = true;
                clickedOnChair = true;
            }
        });
    }
}
