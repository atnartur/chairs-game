export default class Handlers {
    constructor(game) {
        const ws = game.ws;
        ws.on({
            user_logged_count: data => console.log(game.preview.playersCount = data.count),
            user_logged_in: data => game.messages.push(`${data.username} вошел(а)`),
            user_is_first: data => game.preview.isStartButtonHide = data.is_first,
        });
    }
}