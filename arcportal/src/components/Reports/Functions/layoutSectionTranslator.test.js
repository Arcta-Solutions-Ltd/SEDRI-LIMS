import { translateLayoutSectionToContents } from './layoutSectionTranslator';
import { generateLayoutSectionTestData } from './layoutSectionTestData';

const singleColumnFormat = {
    Name: 'SingleColumnOne',
    Type: 'SingleFieldColumnWithSeparateHeading',
    columns: [{ left: 20, width: 520, labelwidth: 80 }],
};

const doubleColumnFormat = {
    Name: 'DoubleColumnFour',
    Type: 'DoubleFieldColumn',
    Columns: [
        { left: 20, width: 255, labelwidth: 120 },
        { left: 315, width: 255, labelwidth: 120 },
    ],
};

describe('translateLayoutSectionToContents', () => {
    it('places location field into Column1', () => {
        const section = {
            Name: 'LocationSection',
            Fields: [{ Label: '@GenLoc@', Value: 'fullyqualifiedname', column: 1, order: 1 }],
        };

        const { contents } = translateLayoutSectionToContents({
            section,
            format: singleColumnFormat,
            dataSection: null,
        });

        expect(contents[0].Column1.Fields).toHaveLength(1);
        expect(contents[0].Column1.Fields[0].Value).toBe('fullyqualifiedname');
    });

    it('places all six cell count fields across two columns', () => {
        const section = {
            Name: 'CellCountSection',
            Dynamic: true,
            Fields: [
                { Label: 'WBC', Value: 'CCWbc', Column: 1, Order: 1 },
                { Label: 'RBC', Value: 'CCRbc', Column: 1, Order: 2 },
                { Label: 'WQ', Value: 'WbcQualitative', Column: 1, Order: 3 },
                { Label: 'RQ', Value: 'RbcQualitative', Column: 2, Order: 4 },
                { Label: 'P', Value: 'Polymorphonuclear', Column: 2, Order: 5 },
                { Label: 'M', Value: 'Mononuclear', Column: 2, Order: 6 },
            ],
        };

        const { contents } = translateLayoutSectionToContents({
            section,
            format: doubleColumnFormat,
            dataSection: null,
        });

        expect(contents[0].Column1.Fields).toHaveLength(3);
        expect(contents[0].Column2.Fields).toHaveLength(3);

        const testData = generateLayoutSectionTestData(contents, null, section);
        expect(testData.Standard).toHaveLength(6);
    });
});
