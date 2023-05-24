import { Size } from "../size"

export class DisplayProductDTO {
    id?: number
    name = ''
    description = ''
    imagePath = ''
    isActive = true
    sizes!: Size[]
}