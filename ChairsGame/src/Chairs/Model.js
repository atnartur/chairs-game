import MK from 'matreshka'

export default class Chair extends MK.Object {
    constructor(data, collection) {
        super();
        this
            .calc(
                'style', 
                ['rotate', 'x', 'y'], 
                (rotate, x, y) => `transform: rotate(-${rotate}deg); left: ${x - collection.widthCenter}px; top: ${y}px;`
            )
            .jset(data)
            .on('render', () => this.bindNode('style', ':sandbox', MK.binders.prop('style')))
            .on('click::sandbox', console.log);
    }
}
