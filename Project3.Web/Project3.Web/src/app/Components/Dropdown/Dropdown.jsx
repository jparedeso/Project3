import React from 'react';

const Dropdown = (props) => {
    const { items, style } = props
    return (
        <div>
            {items.map( item => ( 
                <div>
                    <button className={style} key={item}>
                        {item}
                    </button>
                </div>
               )
            )}
        </div>
    );
};

export default Dropdown;