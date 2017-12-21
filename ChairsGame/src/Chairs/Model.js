import MK from 'matreshka'

export default class Chair extends MK.Object {
    constructor(data, collection) {
        super();
        this
            .addDataKeys(['style', 'x', 'rotate',' y', 'isClicked'])
            .calc(
                'style', 
                ['rotate', 'x', 'y', 'isClicked'], 
                (rotate, x, y, isClicked) => 
                    `transform: rotate(-${rotate}deg); 
                    left: ${x - collection.widthCenter}px; 
                    top: ${y}px; 
                    opacity: ${isClicked ? 0.5 : 1}`
            )
            .jset({
                isClicked: false, 
                ...data
            })
            .on('render', () => this.bindNode('style', ':sandbox', MK.binders.prop('style')))
            .once('click::sandbox', () => {
                if (!this.isClicked) {
                    const number = collection.indexOf(this);
                    collection.trigger('click', number)
                }
            });
    }
}
