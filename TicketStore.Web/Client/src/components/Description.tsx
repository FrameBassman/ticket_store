import React from 'react';
import { PayButton } from './PayButton';
import './Description.css';

export const Description = () => {
  return (
    <div className="description">
       <div className="description__content">
          <div className="description__money">
            <div className="description__content--item description__content--bold">250 ₽</div>
            <PayButton className="description__content--item" />
          </div>
          <div className="description__licence">За один раз можно купить только один билет. Оплата производится на Яндекс.Кошелек. В момент оплаты яндекс потребует уаказать имейл - после оплаты на него придет электронный билет со штрихкодом, который нужно будет показать на входе. Не покупайте билеты с рук, потому что они могут быть с неверным штрихкодом. Сделано с любовью</div>
        </div>
    </div>
  )
}
