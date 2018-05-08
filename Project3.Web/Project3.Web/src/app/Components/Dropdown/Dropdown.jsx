import React from 'react';

const Dropdown = (props) => {
    const { items, style } = props
    return (
        <div>
            {items.map(item => (
                <div key={item}>
                    <button className={style}>
                        {item}
                    </button>
                </div>
               )
            )}
        </div>
    );
};

export default Dropdown;